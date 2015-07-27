using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace WeiboEntityDistinguish
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        String weibo;
        String word;
        Storyboard stdStart, stdEnd, stdMin, stdMax;

        public MainWindow()
        {
            InitializeComponent();

            // 加载开始、结束、最小化、最大化效果
            stdStart = (Storyboard)this.Resources["start"];
            stdEnd = (Storyboard)this.Resources["end"];
            stdMin = (Storyboard)this.Resources["min"];
            stdMax = (Storyboard)this.Resources["max"];

            stdEnd.Completed += (s, e) => Close();
            stdMin.Completed += (s, e) => this.WindowState = System.Windows.WindowState.Minimized;

            this.StateChanged += (s, e) =>
            {
                if (this.WindowState != System.Windows.WindowState.Maximized)
                {
                    stdMax.Begin();
                }
            };
            this.Loaded += (s, e) =>
            {
                stdStart.Begin();
                var loadingAnim = (Storyboard)Resources["loadingAnim"];
                loadingAnim.Begin();
            };
            var list = new ObservableCollection<BaikeTerm>();
            ItemList.ItemsSource = list;
            //list.Add(new BaikeTerm("大是滴是滴暗杀的阿斯顿 ", "大三的 ", "http://baidu.com", "", "阿斯顿 阿斯顿暗杀的阿斯顿全文额 人为飞是地方 我二泉网额  人额外他为人 全文额的是飞大三飞为人 ", 0.4));
            //list.Add(new BaikeTerm("大是滴是滴暗杀的阿斯顿 ", "大三的 ", "", "", "阿斯顿 阿斯顿暗杀的阿斯顿全文额 人为飞是地方 我二泉网额  人额外他为人 全文额的是飞大三飞为人 ", 0.4));
            //list.Add(new BaikeTerm("大是滴是滴暗杀的阿斯顿 ", "大三的 ", "", "", "阿斯顿 阿斯顿暗杀的阿斯顿全文额 人为飞是地方 我二泉网额  人额外他为人 全文额的是飞大三飞为人 ", 0.4));
            //list.Add(new BaikeTerm("大是滴是滴暗杀的阿斯顿 ", "大三的 ", "", "", "阿斯顿 阿斯顿暗杀的阿斯顿全文额 人为飞是地方 我二泉网额  人额外他为人 全文额的是飞大三飞为人 ", 0.4));
            //this.Closing += (s, e) => { stdEnd.Begin(); };
        }

        void fun()
        {

            if (ItemList.Items.Count > 0)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    ((ObservableCollection<BaikeTerm>)ItemList.ItemsSource).Clear();
                }, System.Windows.Threading.DispatcherPriority.Normal);
            }

            if (weibo.IndexOf(word) >= 0)
            {
                //List<BaikeItem> baikeitem_list = HttpUtil.GetMatchBaikeItem(word, weibo);
                List<BaikeTerm> baiketerm_list = Process.GetMatchBaikeTerm(word, weibo);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (baiketerm_list == null)
                    {
                        message.Visibility = System.Windows.Visibility.Visible;
                        message.Content = "百度百科尚未收录词条";
                    }
                    else
                    {
                        for (int i = 0; i < baiketerm_list.Count; i++)
                        {
                            BaikeTerm bt = baiketerm_list[i];
                            //String title = bt.Title;

                            //String description = bt.Description.Length >= 30 ?
                            //   bt.Description.Substring(0, 30) + "..." : bt.Description;

                            //String url = bt.Url;
                            //double match = bt.Match;

                            //String show = title + "\n描述：" + description + "\n匹配权重值" + match;


                            //Label label = new Label() { Margin=new Thickness(5,2,5,2), Content = show, FontSize = 12, Tag = url ,Background = new SolidColorBrush(Colors.Gray) };


                            //label.MouseDoubleClick += (s, e) =>
                            //{
                            //    if (!url.Equals(""))
                            //        System.Diagnostics.Process.Start(url);
                            //};

                            //ItemList.Items.Add(label);
                            ((ObservableCollection<BaikeTerm>)ItemList.ItemsSource).Add(bt);

                        }
                    }

                    //for (int i = 0; i < 10; i++)
                    //{

                    //    String show = "Test" + "\n描述：" + "Test" + "\n匹配权重值" + "0.08";
                    //    String url = "www.baidu.com";
                    //    Label label = new Label() { Content = show, FontSize = 12, Tag = "http://www.baidu.com" };


                    //    label.MouseDoubleClick += (s, e) =>
                    //    {
                    //        if (!url.Equals(""))
                    //            System.Diagnostics.Process.Start(url);
                    //    };

                    //    ItemList.Items.Add(label);

                    //}

                }, System.Windows.Threading.DispatcherPriority.Normal);


            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    message.Visibility = System.Windows.Visibility.Visible;
                    message.Content = "您要查找的命名实体不在微博中";
                    //ItemList.Items.Add(new Label() { Content = "您要查找的命名实体不在微博中", FontSize = 12, Tag = "" });
                }, System.Windows.Threading.DispatcherPriority.Normal);
            }

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                Recognition.IsEnabled = true;
                //ProgressBarConpent.Visibility = Visibility.Hidden;
                HideLoading();
            }, System.Windows.Threading.DispatcherPriority.Normal);
        }
        
        private void ShowLoading(){
            loadingView.Visibility = System.Windows.Visibility.Visible;
            loadingView.BeginAnimation(OpacityProperty, new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.3))));
        }
        
        private void HideLoading(){
            var ani = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(0.3)));
            ani.Completed+= (s,e) => loadingView.Visibility = System.Windows.Visibility.Hidden;
            loadingView.BeginAnimation(OpacityProperty, ani);
        }

        private void Recognition_Click(object sender, RoutedEventArgs e)
        {
            weibo = WeiboTextBox.Text;
            word = EntityTextBox.Text;

            Thread thread = new Thread(fun);
            thread.Start();
            Recognition.IsEnabled = false;
            message.Visibility = System.Windows.Visibility.Hidden;
            ShowLoading();
            //ProgressBarConpent.Visibility = Visibility.Visible;
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            //this.WindowState = System.Windows.WindowState.Minimized;
            stdMin.Begin(this);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            stdEnd.Begin();
            //this.Close();
        }

        private void GotoUrl(object sender, RoutedEventArgs e)
        {
            string url = (string)((Button)sender).Tag;
            if (!url.Equals(""))
                System.Diagnostics.Process.Start(url);
        }


    }
}
